import AuthBox from "../authBox";
import Success from "../../../assets/icons/success.svg?react";
import { toaster } from "../../ui/toaster";
import { Flex, Icon, Text } from "@chakra-ui/react";
import { useNavigate } from "react-router-dom";
import { useEffect } from "react";

const SuccessLogin = () => {
  const navigate = useNavigate();
  useEffect(() => {
    toaster.success({
      duration: 2500,
      description: "سيتم تحويلك للصفحة الرئيسية",
    });
    setTimeout(() => {
      navigate("/");
    }, 2500);
  }, [navigate]);
  return (
    <AuthBox stepper activeSteps={4}>
      <Flex mt={12} direction="column" alignItems={"center"} gap={3}>
        <Text fontWeight={"semibold"}>تم التسجيل بنجاح</Text>
        <Icon>
          <Success />
        </Icon>
      </Flex>
    </AuthBox>
  );
};

export default SuccessLogin;
