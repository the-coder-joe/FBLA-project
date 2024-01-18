import matplotlib.pyplot as plt
import numpy as np
from tensorflow import keras
import tensorflow as tf

data = keras.datasets.fashion_mnist

(train_images, train_labels), (test_images, test_labels) = data.load_data()
print(train_images, train_labels, test_images, test_labels)