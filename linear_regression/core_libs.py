import os
os.environ['TF_CPP_MIN_LOG_LEVEL'] = '3'
import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
import seaborn as sns
import tensorflow as tf
import scipy as sf
from tensorflow import keras
from keras import layers
from scipy import stats
import ssl

ssl._create_default_https_context = ssl._create_unverified_context
