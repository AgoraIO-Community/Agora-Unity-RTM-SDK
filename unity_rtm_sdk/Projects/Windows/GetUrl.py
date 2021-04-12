import sys


arch = sys.argv[1]
config = open('../../url_config.txt', 'r').read()
idx = config.find('WIN_' + {arch})
url = config[config.find('=', idx) + 1: config.find('\n', idx)].strip()

try:
    if sys.argv[2] == 'filename':
        print(url[url.rfind('/') + 1:])
except IndexError:
    print(url)
