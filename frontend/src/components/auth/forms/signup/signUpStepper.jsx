import { Box, Flex, Steps } from "@chakra-ui/react";

const SignupStepper = ({ steps = 4, activeStep = 1 }) => {
  return (
    <Flex justifyContent="center" gap={4} mb={4}>
      <Steps.Root linear step={activeStep} variant={"accent"}>
        <Steps.List>
          {Array.from({ length: steps }).map((_, index) => (
            <Steps.Item key={index} index={index}>
              <Steps.Indicator>
                <Steps.Status
                  incomplete={
                    <Box
                      w={5}
                      h={5}
                      borderWidth={2}
                      borderColor="accent.500"
                      borderRadius="full"
                    />
                  }
                  complete={
                    <Box
                      w={5}
                      h={5}
                      borderRadius={"full"}
                      bgColor={"accent.500"}
                    />
                  }
                />
              </Steps.Indicator>
              <Steps.Separator />
            </Steps.Item>
          ))}
        </Steps.List>
      </Steps.Root>
    </Flex>
  );
};

export default SignupStepper;
