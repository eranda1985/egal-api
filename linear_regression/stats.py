import core_libs as cl
import sys

def get_stats(url):
    raw_dataset = cl.pd.read_csv(url)
    traindataset = raw_dataset.copy()
    statsObject = traindataset.describe();
    print(statsObject.to_json());

get_stats(sys.argv[1])