from fastapi import APIRouter, Depends, status
from fastapi.responses import JSONResponse
from helpers.config import get_settings, Settings
from controllers import RecommendationController
from qdrant_client import QdrantClient



user_recommendation_router = APIRouter(
    prefix="/api/AI/user",
    tags=["AI", 'user']
)

@user_recommendation_router.post("/recommendation")
async def user_recommendation(query: str,
                            app_settings: Settings = Depends(get_settings)):

    qdrant_client = QdrantClient(
        url=app_settings.QDRANT_API_URL,
        api_key=app_settings.QDRANT_API_KEY
    )

    recommendation_controller = RecommendationController(qdrant_client)
    query_emb = recommendation_controller.convert_into_embeddings(query)
    recommendation_result = recommendation_controller.embeddings_search(query_emb)

    return JSONResponse(status_code=status.HTTP_200_OK,
                        content=recommendation_result)