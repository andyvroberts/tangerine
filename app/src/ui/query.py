
import logging
import os

log = logging.getLogger("app.src.ui.query.py")
#---------------------------------------------------------------------------------------#   
def get_settlement_month():
    """interact with the UI to find the query month

        Args:
            None:
            Return: a tuple YYYY, mm
    """
    return 2023, 12

#---------------------------------------------------------------------------------------#   
def get_price_file_name(file_type, year_month):
    """interact with the UI to find the required file path

        Args:
            file_type: integer file type.  1 = system price, etc.
            Return: the fully qualified file path and name.
    """
    home_dir = os.path.expanduser('~') + os.path.sep + 'downloads' + os.path.sep

    match file_type:
        case 1:
            return f'{home_dir}SysPrice-{year_month}.parquet';
        case 2:
            return f'{home_dir}SysVol-{year_month}.parquet';
    
    return home_dir

#---------------------------------------------------------------------------------------#   
def get_price_url():
    """interact with the UI to find the imbalance price function URL

        Args:
            None:
            Return: the url of the locally running function app
    """
    return ' http://localhost:7071/api/bmrs/price'

#---------------------------------------------------------------------------------------#   
def get_volume_url():
    """interact with the UI to find the imbalance volume function URL

        Args:
            None:
            Return: the url of the locally running function app
    """
    return ' http://localhost:7071/api/bmrs/volume'


