from qdrant_client import models, QdrantClient
from ..VectorDBInterface import VectorDBInterface
from models.enums.VectorDBEnums import DistanceMetodEnums
import logging

class VectorDBProvider(VectorDBInterface):
    def __init__(self, db_path: str, distance_method: str):
        
        self.client = None  # connect function
        self.db_path = db_path

        self.distance_method = None
        if distance_method == DistanceMetodEnums.COSINE.value:
            self.distance_method = models.Distance.COSINE
        elif distance_method == DistanceMetodEnums.DOT.value:
            self.distance_method = models.Distance.DOT

        self.logger = logging.getLogger(__name__)

    def connect(self):
        self.client = QdrantClient(path=self.db_path)

    def disconnect(self):
        self.client = None

    def conn