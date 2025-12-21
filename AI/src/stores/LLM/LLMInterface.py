from abc import ABC, abstractmethod

# abstract class
# write the structure of the class without implmentation
class LLMInterface(ABC):
    
    
    @abstractmethod # this function must be implmented or used in child classes
    def set_generation_model(self, model_id: str):
        pass

    @abstractmethod
    def set_embedding_model(self, model_id: str, embedding_size: int):
        pass

    @abstractmethod
    def generate_text(self, prompt: str, chat_history: list=[], max_output_tokens: int=None,
                      temprature: float = None):
        pass

    @abstractmethod
    def embed_text(self, text, input_type: str=None):
        # document type changes how the llm will embed the text
        pass

    @abstractmethod
    def construct_prompt(self, prompt: str, role: str):
        pass