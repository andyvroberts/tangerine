import logging
import pyarrow.parquet as pq

log = logging.getLogger("app.src.store.files.py")
#------------------------------------------------------------------------------
def write_table(tab, location):
    """
        write a local parquet file using the built-in pyarrow tools

        Args:
            tab: an already created parquet table object
            location: the path and file name
            Return: None
    """
    log.debug(f'writing parquet file to {location}.')
    pq.write_table(tab, location)
 