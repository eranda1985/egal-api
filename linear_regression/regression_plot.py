from cProfile import label
from turtle import color
import core_libs as cl
import io
from PIL import Image
import base64
import sys
import lr

def plot_regression_graph(url):
    train_features, training_labels = lr.get_training_sets(url)
    x_dimension_trained_model = lr.linear_regression_model(url)

    x = cl.np.array(cl.tf.linspace(0.0, 100, 101))
    y = cl.np.array(x_dimension_trained_model.predict(x)).reshape(-1)

    fig = cl.plt.figure()
    cl.plt.plot(x,y, color='k', label='Result')
    cl.plt.scatter(train_features['x'][:500], training_labels[:500], label='Data')
    fig.canvas.draw()
	
	#open a byte stream
    img_buf = io.BytesIO()
	
    cl.plt.savefig(img_buf, format='png')
    img_buf.seek(0)
	
    print(base64.b64encode(img_buf.getvalue()).decode())
    img_buf.close()

plot_regression_graph(sys.argv[1])

