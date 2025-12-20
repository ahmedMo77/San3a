import { Box, Flex } from "@chakra-ui/react";
import Tabs from "../../../sharedComponents/Tabs";
import SignupStepper from "../forms/signup/signUpStepper";

const AuthBox = ({ children, stepper, activeSteps }) => {
  return (
    <Flex justifyContent={"center"}>
      <Box w="520px" minH="520px" bg="white" boxShadow="md" rounded="lg" p={6}>
        <Tabs />
        {stepper && <SignupStepper activeStep={activeSteps} />}
        {children}
      </Box>
    </Flex>
  );
};

export default AuthBox;
