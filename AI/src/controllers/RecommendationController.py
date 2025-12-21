from .BaseController import BaseController
from sentence_transformers import SentenceTransformer
from qdrant_client import QdrantClient

class RecommendationController(BaseController):
    def __init__(self, client: QdrantClient):
        super().__init__()
        self.client = client


    def convert_into_embeddings(self, query: str):
        model = SentenceTransformer(self.app_settings.MODEL_NAME)
        embeddings = model.encode(query).tolist()
        return embeddings
    
    
    def embeddings_search(self, query_emb: list):

        result = self.client.search(
            collection_name="craftsmen",
            query_vector=("embeddings", query_emb),
            limit=15
        )

        return [r.payload["describtion"] for r in result]
