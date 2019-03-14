import pandas as pd
import numpy as np
from collections import namedtuple

def myiter(d, cols=None):
    if cols is None:
        v = d.values.tolist()
        cols = d.columns.values.tolist()
    else:
        j = [d.columns.get_loc(c) for c in cols]
        v = d.values[:, j].tolist()

    n = namedtuple('MyTuple', cols)

    for line in iter(v):
        yield n(*line)


list = ['Cardio', 'Smoking','Alcohol','Diabetes','Alzheimers','Cancer','Obesity','Arthritis','Asthma','Stroke']
data = {'Cardio':'317', 'Smoking':'300','Alcohol':'249','Diabetes':'245','Alzheimers':'223','Cancer':'171','Obesity':'147','Arthritis':'128','Asthma':'56','Stroke':'33'}
pd.DataFrame.from_dict(data, orient='index')
print(pd.DataFrame)
with pd.option_context('display.max_rows', None, 'display.max_columns', None):
    print(pd.DataFrame)
df = pd.DataFrame
print(df)


# Take a 2D array as input to your DataFrame 
my_2darray = np.array([[1, 2, 3], [4, 5, 6]])

# Take a dictionary as input to your DataFrame 

my_dict = {1: ['1', '3'], 2: ['1', '2'], 3: ['2', '4']}


# Take a DataFrame as input to your DataFrame 

my_df = pd.DataFrame(data=[4,5,6,7], index=range(0,4), columns=['A'])


# Take a Series as input to your DataFrame

my_series = pd.Series({"Belgium":"Brussels", "India":"New Delhi", "United Kingdom":"London", "United States":"Washington"})


from collections import namedtuple
