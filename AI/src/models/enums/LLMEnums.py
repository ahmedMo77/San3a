from enum import Enum

class LLMEnums(Enum):
    # put providers here
    OPENAI = "OPENAI"
    COHERE = "COHERE"

class OpenAIEnums(Enum):
    SYSTEM = "system"
    ASSISTANT = "assistant"
    USER = "user"

class CohereEnums(Enum):
    USER = "user"
    ASSISTANT = "assistant"
    SYSTEM = "system"
    TOOL = "tool"

    QUERY = "search_query"
    DOCUMENT = "search_document"

class DocumentTypeEnums(Enum):
    DOCUMENT = "document"
    QUERY = "query"