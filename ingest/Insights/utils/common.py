import calendar
import json
import ast
import urllib.request as ur

def days_in_month(y: int, m: int):
    '''
    given a year and a month, return each day as a string.

    params:
        y: integer year yyyy
        m: integer month m or mm
        return: string yyyy-mm-dd
    '''
    for d in range (1, calendar.monthrange(y, m)[1]):
        date_string = (f'{y}-{m:02d}-{d:02d}')
        yield date_string


def get_data_from_payload(url):
    '''
    given an Elexon bmrs API, return the 'data' part of the payload

    params:
        url: the full URL to execute
        return: python list (of dicts)
    '''
    webReq = ur.urlopen(url)
    webData = webReq.read()
    webDataEncoding = webReq.info().get_content_charset('utf-8')
    jsonObj = json.loads(webData.decode(webDataEncoding))
    payload = jsonObj['data']

    return payload

