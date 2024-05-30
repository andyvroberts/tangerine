import logging
import pyarrow.parquet as pq
import pyarrow as pa

log = logging.getLogger("app.src.store.parquet.py")
#------------------------------------------------------------------------------
def system_prices(data):
    """
        format the decoded data into a parquet buffer stream so it can be written
        to a file.

        Args:
            data: a list of dicts representing all the denormalized records
            location: the path and file name
            Return: number of parquet table rows
    """
    log.debug(f'creating parquet file with columns: {data[0].keys()}')
    
    # convert the python dict into pyarrow columnar arrays with schema metadata
    parq_arr = {
        'SettlementDate' : pa.array([c1['SettlementDate'] for c1 in data], type='string'),
        'SettlementPeriod' : pa.array([c2['SettlementPeriod'] for c2 in data], type=pa.int32()),
        'ImbalanceType' : pa.array([c3['ImbalanceType'] for c3 in data], type='string'),
        'TimeSeriesId' : pa.array([c4['TimeSeriesId'] for c4 in data], type='string'),
        'BusinessType' : pa.array([c5['BusinessType'] for c5 in data], type='string'),
        'ControlArea' : pa.array([c6['ControlArea'] for c6 in data], type='string'),
        'PriceGbp' : pa.array([c7['PriceGbp'] for c7 in data], type=pa.float32()),
        'PriceCategory': pa.array([c8['PriceCategory'] for c8 in data], type='string'),
        'CurveType': pa.array([c9['CurveType'] for c9 in data], type='string'),
        'Resolution': pa.array([c10['Resolution'] for c10 in data], type='string'),
        'DocumentType': pa.array([c11['DocumentType'] for c11 in data], type='string'),
        'ProcessType': pa.array([c12['ProcessType'] for c12 in data], type='string'),
        'ActiveFlag': pa.array([c13['ActiveFlag'] for c13 in data], type='string'),
        'Status': pa.array([c14['Status'] for c14 in data], type='string'),
        'DocumentId': pa.array([c15['DocumentId'] for c15 in data], type='string'),
        'RevisionNumber': pa.array([c16['RevisionNumber'] for c16 in data], type=pa.int32()),
    }

    # convert pyarrow arrays to the compressed parquet table
    table = pa.Table.from_pydict(parq_arr)
    # if you want to create a memory buffer do this:
    #buf = pa.BufferOutputStream()
    #pq.write_table(table, buf)
    #buf_bytes = buf.getvalue().to_pybytes()
    
    log.info(f'Created table with {table.num_rows} rows.')

    #return buf_bytes
    return table