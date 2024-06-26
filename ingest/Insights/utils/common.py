import calendar
import json
import ast
import urllib.request as ur
import pyarrow as pa
import pyarrow.parquet as pq

def days_in_month(y: int, m: int):
    '''
    given a year and a month, generate each day as a date string.

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


def get_payload(url):
    '''
    given an Elexon bmrs API, return the payload

    params:
        url: the full URL to execute
        return: python list (of dicts)
    '''
    webReq = ur.urlopen(url)
    webData = webReq.read()
    webDataEncoding = webReq.info().get_content_charset('utf-8')
    jsonObj = json.loads(webData.decode(webDataEncoding))
    payload = jsonObj

    return payload


def create_parquet_buff(data, meta_data):
    '''
    given a list of dicts and a list of column meta-data, create a parquet table.

    params:
        data: a list of dicts representing one or many homogenous records.
        meta_data: a list of field metadata: a) the field name b) the field type
        return: memory buffer
    '''
    field_types = []

    for field_name, data_type in meta_data:
        if data_type == 1:
            field_types.append(pa.field(field_name, pa.string()))
        if data_type == 2:
            field_types.append(pa.field(field_name, pa.int32()))
        if data_type == 3:
            field_types.append(pa.field(field_name, pa.float32()))
        if data_type == 4:
            field_types.append(pa.field(field_name, pa.bool_()))

    tab_schema = pa.schema(field_types)
    tab = pa.Table.from_pylist(data, schema=tab_schema)

    buff = pa.BufferOutputStream()
    pq.write_table(tab, buff)
    buff_bytes = buff.getvalue().to_pybytes()
    
    return buff_bytes

#with io.BytesIO() as buffer:
#    pq.write_table(table, buffer)
