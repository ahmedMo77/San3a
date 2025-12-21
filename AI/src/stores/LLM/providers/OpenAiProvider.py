from LLMInterface import LLMInterface
from models.enums.LLMEnums import LLMEnums, DocumentTypeEnums
from openai import OpenAI
import logging


class OpenAiProvider(LLMInterface):

    def __init__(self, api_key: str, api_url: str=None,
                max_input_characters: int=1000,
                max_output_characters: int=1000,
                temprature: float=0.2):
        
        self.api_key = api_key
        self.api_url = api_url

        self.max_input_characters = max_input_characters
        self.max_output_characters = max_output_characters
        self.temprature = temprature

        self.generation_model_id = None

        self.embedding_model_id = None
        self.embedding_model_size = None

        self.client = OpenAI(
            api_key=self.api_key,
            api_url=self.api_url
        )

        # logs of this file
        self.logger = logging.getLogger(__name__)


    def set_generation_model(self, model_id: str):
        # عشان اعرف اغيره في اي وقت في الرن تايم
        self.generation_model_id = model_id


    def generate_text(self, prompt: str, chat_history: list=[], max_output_tokens: int=None,
                        temprature: float = None):
        
        if not self.client: 
            self.logger.error("openai client was not set")

        if not self.generation_model_id: 
            self.logger.error("generation model was not set")

        max_output_tokens = self.max_output_characters if not max_output_tokens else max_output_tokens
        temprature = self.temprature if not temprature else temprature
        processed_prompt = self.process_prompt(prompt)

        chat_history.append(self.construct_prompt(prompt=processed_prompt, role=OpenAIEnums.USER.value))
        response = self.client.chat.completions.create(
            model=self.generation_model_id,
            messages=chat_history,
            max_tokens=max_output_tokens,
            temperature=temprature
        )

        if not response or not response.choices or len(response.choices) == 0 or not response.choices[0].message:
            self.logger.error("error while generation with openai")
            return None
        
        return response.choices[0].message['content']
    

    def construct_prompt(self, prompt: str, role: str):
        return {
            "role": role,
            "content":prompt
        }
    
    
    def process_prompt(self, prompt: str):
        return prompt[:self.max_input_characters].strip()

    def set_embedding_model(self, model_id: str, embedding_size: int):
        self.embedding_model_id = model_id
        self.embedding_model_size = embedding_size

    def embed_text(self, text: str, input_type: str=None):
        if not self.client: 
            self.logger.error("openai client was not set")
        if not self.embedding_model_id: 
            self.logger.error("embedding model was not set")

        processed_text = self.process_prompt(text)

        response = self.client.embeddings.create(
            model=self.embedding_model_id,
            input=processed_text,
            document_type=input_type
        )

        if not response or not response.data or len(response.data)  == 0 or not response.data[0].embedding:
            self.logger.error("error while embedding with openai")
            return None
        
        return response.data[0].embedding