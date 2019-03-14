from urllib import request
url = "https://www.nltk.org/book/ch03.html"
response = request.urlopen(url)
raw = response.read().decode('utf8')
type(raw)