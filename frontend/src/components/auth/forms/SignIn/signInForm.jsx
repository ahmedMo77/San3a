import { Flex, Link, Box } from "@chakra-ui/react";
import { Link as RouterLink } from "react-router-dom";
import ButtonContainer from "../../../../sharedComponents/Button";
import GoogleIcon from "../../../../assets/icons/google.svg?react";
import { FormInput } from "../../../../sharedComponents/Forms";

const SignInForm = () => {
  return (
    <>
      <Flex direction="column" gap={4}>
        <FormInput
          label="البريد الإلكتروني / رقم التليفون"
          placeholder="البريد الالكتروني/ رقم التليفون"
          name="username"
        />
        <FormInput
          label="كلمة السر"
          placeholder="كلمة السر"
          name="password"
          type="password"
        />
      </Flex>

      <Link
        as={RouterLink}
        to="/forgot-password"
        py={3}
        color="heading.500"
        fontSize="sm"
      >
        نسيت كلمة المرور؟
      </Link>

      <ButtonContainer
        direction="column"
        primaryText="Log in with Google"
        secondaryText={"تسجيل الدخول"}
        primaryIcon={<Box as={GoogleIcon} boxSize={5} />}
      />

      <Link
        as={RouterLink}
        to={"/إنشاء-حساب"}
        _focus={{ textDecoration: "none", outline: "none" }}
        py={3}
        color="header.500"
        fontSize="sm"
      >
        ما عندكش حساب؟ سجل دلوقتي
      </Link>
    </>
  );
};

export default SignInForm;
