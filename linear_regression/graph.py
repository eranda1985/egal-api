import core_libs as cl
import io
from PIL import Image
import base64
import sys
from lr import get_training_sets

def plot_graph(url):
	#url = 'https://drive.google.com/u/0/uc?id=1L6RErnw0Xb_R_ZKJ_s6zbcGePmirEg-r&export=download'

	train_feat, train_labels = get_training_sets(url)

	# initiate a figure object
	fig = cl.plt.figure()
	# create a scatter graph and draw it onto the canvas. 
	cl.plt.scatter(train_feat['x'][:500], train_labels[:500], label='Data')
	cl.plt.xlabel('x')
	cl.plt.ylabel('y')
	cl.plt.legend()
	fig.canvas.draw()

	# we can create an (PIL) image object using the canvas. 
	#img_object = Image.frombytes('RGB',fig.canvas.get_width_height(),fig.canvas.tostring_rgb())
	
	#open a byte stream
	img_buf = io.BytesIO()
	
	# we can also fill the input stream with image data from plot. 
	cl.plt.savefig(img_buf, format='png')
	img_buf.seek(0)
	
	print(base64.b64encode(img_buf.getvalue()).decode())
	img_buf.close()
	#img_object.show()
	#plt.plot(x, y, color='k', label='Predictions')

plot_graph(sys.argv[1])