import azure.functions as func
from utils import common
import os
import time
import logging

sysprice = func.Blueprint()

@sysprice.route(route="prices/{year}/{month}", methods=[func.HttpMethod.GET])
async def get_system_price(req: func.HttpRequest) -> str:
    lake_writer = os.environ['WriteTo']
    
    if lake_writer == 'Lake':
        import utils.lake_writer as wrw
    else:
        import utils.local_writer as wrw

    _year = req.route_params.get('year')
    _month = req.route_params.get('month')

    url = 'https://data.elexon.co.uk/bmrs/api/v1/balancing/settlement/system-prices/'

    mm = int(_month)
    yyyy = int(_year)
    system_prices = []

    # read all the prices from Elexon API
    for date_string in common.days_in_month(yyyy, mm):
        run_url = f'{url}{date_string}'
        logging.debug(date_string)
        price_list = common.get_data_from_payload(run_url)
        system_prices.extend(price_list)
        time.sleep(1.2)

    logging.info(f'price records: {len(system_prices)}.')

    # create the metadata for parquet conversion.
    # 1 = string, 2 = int32(), 3 = float32(), 4 = bool_()
    meta_data = [
        ['settlementDate', 1],
        ['settlementPeriod', 2],
        ['startTime', 1],
        ['createdDateTime', 1],
        ['systemSellPrice', 3],
        ['systemBuyPrice', 3],
        ['bsadDefaulted', 4],
        ['priceDerivationCode', 1],
        ['reserveScarcityPrice', 3],
        ['netImbalanceVolume', 3],
        ['sellPriceAdjustment', 3],
        ['buyPriceAdjustment', 3],
        ['replacementPrice', 3],
        ['replacementPriceReferenceVolume', 3],
        ['totalAcceptedOfferVolume', 3],
        ['totalAcceptedBidVolume', 3],
        ['totalAdjustmentSellVolume', 3],
        ['totalAdjustmentBuyVolume', 3],
        ['totalSystemTaggedAcceptedOfferVolume', 3],
        ['totalSystemTaggedAcceptedBidVolume', 3],
        ['totalSystemTaggedAdjustmentSellVolume', 3],
        ['totalSystemTaggedAdjustmentBuyVolume', 3]
    ]
    pq_data = common.create_parquet_buff(system_prices, meta_data)
    logging.debug(f'parquet buffer size = {len(pq_data)}.')

    file_name = f'{yyyy}{mm:02d}_disebsp.parquet'

    _ = wrw.save_file(pq_data, file_name)

    msg = f'Get system prices for {_year}-{_month}. Found {len(system_prices)} records.'
    logging.info(msg)
    return msg