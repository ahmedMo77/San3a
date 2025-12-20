import { Flex, Text } from "@chakra-ui/react";
import BoxContainer from "../../../sharedComponents/boxContainer";
import { useNavigate } from "react-router-dom";

const RegisterWorkerCard = ({ faded, onHover, onLeave }) => {
  const navigate = useNavigate();
  return (
    <BoxContainer
      height={56}
      width={56}
      bgColor="heading.500"
      rounded="2xl"
      cursor="pointer"
      opacity={faded ? 0.5 : 1}
      transition="opacity 0.2s ease"
      onMouseEnter={onHover}
      onMouseLeave={onLeave}
      onClick={() => {
        navigate("/إنشاء-حساب/حرفي");
      }}
    >
      <Flex direction="column" alignItems="center" gap={4}>
        <Text fontWeight="bold" fontSize="2xl" color="white">
          حرفي
        </Text>
        <Text width="70%" textAlign="center" fontSize="sm" color="white">
          إنشاء حساب علشان تقدّم خدماتك.
        </Text>
      </Flex>
    </BoxContainer>
  );
};

export default RegisterWorkerCard;
