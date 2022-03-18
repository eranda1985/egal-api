import core_libs as cl
import sys
import json

# Method to calculate linear regression
def get_linear_regression(url):
	#url = 'https://drive.google.com/u/0/uc?id=1dPTU5OBkhY3g-3CcLOVXT90dL8ja_VvX&export=download'

	train_features, train_labels = get_training_sets(url)

	# always plot a histogram and see if it's showing a proper bell shape without too much leaning to either side. 
	#plt.hist(train_features['x'], bins=10)
	
	# normalise the x dimension (bring it to common scale)
	x_dim = cl.np.array(train_features['x'])
	x_dim_normalizer = cl.layers.Normalization(input_shape=[1,], axis=None)
	x_dim_normalizer.adapt(x_dim)

	x_dim_model = cl.tf.keras.Sequential([
	    x_dim_normalizer,
	    cl.layers.Dense(units=1)
	    ])

	#x_dim_model.summary()

	x_dim_model.compile(optimizer=cl.tf.optimizers.Adam(learning_rate=0.1), loss='mean_absolute_error')

	history = x_dim_model.fit(
		train_features['x'],
		train_labels,
		epochs=100,
		# Suppress logging.
		verbose=0,
		# Calculate validation results on 20% of the training data.
		validation_split = 0.2)


	x = cl.np.array(cl.tf.linspace(0.0, 5, 6))
	y = cl.np.array(x_dim_model.predict(x)).reshape(-1)
	y_diff = y[1:2] - y[:1]
	x_diff = x[1:2] - x[:1]

	jsonstr =  json.dumps({
		'y_interceptor': float(y[:1]), 
		'coefficent':  float(y_diff/x_diff), 
		'x_samples': x.tolist(), 
		'y_samples': y.tolist()})
	
	print(jsonstr)

	#cl.plt.plot(x,y, color='k', label='Result')
	#cl.plt.scatter(train_features['x'][:500], train_labels[:500], label='Data')

def check_parameter_length_is_valid(x_dim, y_dim):
	if(x_dim.size > 500 or y_dim.size > 500):
		return False
	return True

def check_parameter_is_null(x_dim, y_dim):
	if(x_dim is None or y_dim is None):
		return True
	return False

def check_paramter_is_numeric(x_dim, y_dim):
	return (cl.pd.api.types.is_numeric_dtype(x_dim) and cl.pd.api.types.is_numeric_dtype(y_dim))

# method to get training sets. 
def get_training_sets(url):
	if(url is None):
		print('input url cannot be empty.')
		sys.exit()

	raw_dataset = cl.pd.read_csv(url)
	traindataset = raw_dataset.copy()
	train_features = traindataset.copy()

	# below are some input sanitization. 
	if(check_parameter_is_null(train_features['x'], train_features['y'])):
		print('input x and y dimensions cannot be null.')
		sys.exit()

	if not(check_paramter_is_numeric(train_features['x'], train_features['y'])):
		print('one of the input dimensions are not numeric. Please make sure both x and y dimensions are numeric.')
		sys.exit()

	# remove outliers from x dimension using z-score. Plot histogram first to check if its looking okay.
	train_features['x_z_score'] = cl.np.abs(cl.stats.zscore(train_features['x']))  
	train_features = train_features[train_features['x_z_score'] <=3]
	train_features = train_features[train_features['x'] != 0]
	
	if not (check_parameter_length_is_valid(train_features['x'], train_features['y'])):
		#print('parameters are greater than 500. This program will take the first 500 elements into account.')
		train_features = train_features.head(500)
		#print('new size for train features: ', train_features['x'].size)

	# train_laabels is the target dimension that we want to predict. 
	train_labels = train_features.pop('y')

	return train_features, train_labels

# this is the main method. This won't run when called this module in another python script.
def main():
	# The first parameter passed to this module is the url for csv file. 
	if(len(sys.argv) < 2):
		print('please provide a url for csv data.')
		sys.exit()

	input_url = str(sys.argv[1])
	get_linear_regression(input_url)

if(__name__) == "__main__":
	main()