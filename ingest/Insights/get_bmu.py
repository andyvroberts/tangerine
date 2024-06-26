import azure.functions as func
from utils import common
import os
import datetime
import logging

bmunits = func.Blueprint()

@bmunits.route(route="bmu", methods=[func.HttpMethod.GET])
async def get_bm_units(req: func.HttpRequest) -> str:
    lake_writer = os.environ['WriteTo']
    
    if lake_writer == 'Lake':
        import utils.lake_writer as wrw
    else:
        import utils.local_writer as wrw

    _year = req.route_params.get('year')
    _month = req.route_params.get('month')

    url = 'https://data.elexon.co.uk/bmrs/api/v1/reference/bmunits/all'

    # read all the bm units from Elexon API
    bmu_list = common.get_payload(url)

    logging.info(f'price records: {len(bmu_list)}.')

    # create the metadata for parquet conversion.
    # 1 = string, 2 = int32(), 3 = float32(), 4 = bool_()
    meta_data = [
        ['nationalGridBmUnit', 1],
        ['elexonBmUnit', 1],
        ['eic', 1],
        ['fuelType', 1],
        ['leadPartyName', 1],
        ['bmUnitType', 1],
        ['fpnFlag', 4]
    ]
    pq_data = common.create_parquet_buff(bmu_list, meta_data)
    logging.debug(f'parquet buffer size = {len(pq_data)}.')

    now = datetime.date.today()
    _year = now.year
    _month = now.month
    file_name = f'{_year}{_month:02d}_disebsp.parquet'

    _ = wrw.save_file(pq_data, file_name)

    msg = f'Get BM Units on {_year}-{_month}. Found {len(bmu_list)} records.'
    logging.info(msg)
    return msg