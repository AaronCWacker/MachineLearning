path = "../input"
all_files = glob.glob(path + "/*.csv")


def plot_null_and_unique_values(df):
    f, (ax1) = plt.subplots(1, sharey=True, figsize=(30, 10))
    columns = df.columns
    sns.set(font_scale=1.5)
    res = pd.DataFrame(
        {
            'unique_counts': df[columns].nunique(),
            'null_counts': df[columns].isnull().sum()
        }
    )
    res.sort_values(by='unique_counts', ascending=False, inplace=True)
    sns.barplot(
        y=res.index,
        x=res['unique_counts'].values,
        orient='h',
        ax=ax1
    )
    ax1.axvline(x=len(df), color='red')
    plt.suptitle(
        'The general look of columns\n\
        (vertical red line shows the number of records in the dataset)'
    )
    ax1.set_title('The number of unique values per column')
    plt.show()
    
li = []
for filename in all_files:
    df = pd.read_csv(filename, index_col=None, header=0)
    df.info()
    size = df.size 
    print(df.head(2))
    print(df.tail(2))
    plot_null_and_unique_values(df)
    li.append(df)
    
    #method to recommend relevant questions to the professionals who are most likely to answer them
    
questions = pd.read_csv('../input/questions.csv')
answers = pd.read_csv('../input/answers.csv')
professionals = pd.read_csv("../input/professionals.csv")

def merging(df1, df2, left, right):
    return df1.merge(df2, how="inner", left_on=left, right_on=right)

qa = merging(questions, answers, "questions_id", "answers_question_id")
qa.dop(columns=['questions_id', 'answers_question_id'])
qa.head(500).T

proqa = merging(qa, professionals, "professionals_id", "answers_author_id")
proqa.head(500).T
