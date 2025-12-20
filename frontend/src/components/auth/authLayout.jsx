import { Box, Flex } from "@chakra-ui/react";

const AuthLayout = ({ children }) => {
  return (
    <Box mt={16} mx={10}>
      <Flex justifyContent="space-around" gap={8}>
        {children}
      </Flex>
    </Box>
  );
};

export default AuthLayout;
