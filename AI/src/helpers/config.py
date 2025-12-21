from pydantic_settings import BaseSettings, SettingsConfigDict

class Settings(BaseSettings):

    APP_NAME: str
    APP_VERSION: str
    
    MODEL_NAME: str

    QDRANT_API_URL: str
    QDRANT_API_KEY: str

    GENERATION_BACKEND: str
    EMBEDDING_BACKEND: str

    OPENAI_API_KEY: str
    OPENAI_API_URL: str

    COHERE_API_KEY:str

    GENERATION_MODEL_ID: str
    EMBEDDING_MODEL_ID: str
    EMBEDDING_SIZE: float

    class Config:
        env_file = ".env"

def get_settings():
    return Settings()
