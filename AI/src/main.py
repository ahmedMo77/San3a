from fastapi import FastAPI
from routes import BaseRoute, UserRecommendationRoute
from helpers.config import get_settings
from stores.LLM.LLMProviderFactory import LLMProviderFactory
app = FastAPI()

@app.on_event("startup")
async def startup_client():
    app_settings = get_settings()

    llm_provider_factory = LLMProviderFactory(app_settings)

    # generation client
    app.generation_client = llm_provider_factory.create(provider=app_settings.GENERATION_BACKEND.value)
    app.generation_client.set_generation_model(model_id=app_settings.GENERATION_MODEL_ID.value)

    # embedding client
    app.embedding_client = llm_provider_factory.create(provider=app_settings.EMBEDDING_BACKEND.value)
    app.embedding_client.set_embedding_model(model_id=app_settings.EMBEDDING_MODEL_ID.value,
                                            embedding_size=app_settings.EMBEDDING_SIZE.value)


app.include_router(BaseRoute.base_router)
app.include_router(UserRecommendationRoute.user_recommendation_router)