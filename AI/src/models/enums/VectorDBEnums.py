from enum import Enum

class VectorDBEnums(Enum):
    QDRANT = "QDRANT"

class DistanceMetodEnums(Enum):
    COSINE = 'cosine'
    DOT = "dot"