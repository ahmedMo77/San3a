from LLMInterface import LLMInterface
from models.enums.LLMEnums import LLMEnums, DocumentTypeEnums
import cohere
import logging


class CohereProvider(LLMInterface):

    def __init__(self, api_key: str, 
                max_input_characters: int=1000,
                max_output_characters: int=1000,
                temprature: float=0.2):
        
        self.api_key = api_key

        self.max_input_characters = max_input_characters
        self.max_output_characters = max_output_characters
        self.temprature = temprature

        self.client = cohere.client(
            api_key=self.api_key
        )

        self.generation_model_id = None
        
        self.embedding_model_id = None
        self.embedding_size = None

        self.logger = logging.getLogger(__name__)

    def set_embedding_model(self, model_id: str, embedding_size: str):
        self.embedding_model_id = model_id
        self.embedding_size = embedding_size

    def set_generation_model(self, model_id):
        self.generation_model_id = model_id

    def process_prompt(self, prompt: str):
        return prompt[:self.max_input_characters].strip()
    
    def process_embedding_text(self, input: list, type: str):
        content_list = [{'type': type, type : i} for i in input]
        return [{
            "content": content_list
        },]
    
    def construct_prompt(self, prompt, role):
        return {
            "role": role,
            "content": prompt
        }

    def embed_text(self, text: list, input_type: str="text"):
        if not self.client:
            self.logger.error("Cohere client was not set")
        
        if not self.embedding_model_id:
            self.logger.error("Embedding model was not set")
        
        text_inputs = self.process_embedding_text(input=text, type=input_type)
        
        type = CohereEnums.DOCUMENT.value
        if input_type == DocumentTypeEnums.QUERY.value:
            type = CohereEnums.QUERY.value

        response = self.client.embed(
            inputs=text_inputs,
            model=self.embedding_model_id,
            input_type=type,
            embedding_type=["float"],
        )
        
        if not response or not response.embeddings or not response.embeddings.float:
            self.logger.error("Error while embed texts")
        
        return response.embeddings.float
    
    def generate_text(self, prompt, chat_history: list=[], max_output_tokens = None, temprature = None):
        if not self.client:
            self.logger.error("Cohere client was not set")
        
        if not self.generation_model_id:
            self.logger.error("Generation model was not set")

        max_output_tokens = max_output_tokens if max_output_tokens else self.max_output_characters
        temprature = temprature if temprature else self.temprature
        processed_prompt = self.process_prompt(prompt=prompt)

        response = self.client.chat(
            model=self.generation_model_id,
            chat_history=chat_history,
            message=processed_prompt,
            temprature=temprature,
            max_tokens=max_output_tokens
        )

        if not response or not response.message or not response.message.content or not response.message.content.text:
            self.logger.error("Error while generating text")

        return response.message.content.text
        
        