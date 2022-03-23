import core_libs as cl
from lr import get_training_sets
import io
from PIL import Image
import base64
import sys

def histogram(url):
    train_feat, train_labels = get_training_sets(url)
    fig = cl.plt.figure()
	
    cl.plt.hist(train_feat['x'], bins=20)
    fig.canvas.draw()
	
	#open a byte stream
    img_buf = io.BytesIO()
	
    cl.plt.savefig(img_buf, format='png')
    img_buf.seek(0)
	
    print(base64.b64encode(img_buf.getvalue()).decode())
    img_buf.close()

histogram(sys.argv[1])