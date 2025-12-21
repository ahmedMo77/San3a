from abc import ABC, abstractmethod
from typing import List

class VectorDBInterface(ABC):
    
    @abstractmethod
    def connect(self):
        pass

    @abstractmethod
    def disconnect(self):
        pass

    @abstractmethod
    def is_collection_existed(self, connection_name: str) -> bool:
        pass

    @abstractmethod
    def get_connections_list(self) -> List:
        pass

    @abstractmethod
    def get_connection_info(self, connection_name: str) -> dict:
        pass

    @abstractmethod
    def delete_collection(self, connection_name: str):
        pass

    @abstractmethod
    def create_collection(self, collection_name: str,
                          embedding_size: int,
                          do_reset: bool=False):
        pass

    @abstractmethod
    def insert_one(self, collection_name: str,
                   text: str, vector: list,
                    record_id: int=None, payload: dict=None):
        
        pass

    @abstractmethod
    def insert_many(self, collection_name: str,
                   list_of_text: list[str], list_of_vector: list[list],
                    list_of_record_id: int=None, list_of_payload: dict=None):
        
        pass

    @abstractmethod
    def search_by_vector(self, collection_name: str, vector: list, limit: int):
        pass