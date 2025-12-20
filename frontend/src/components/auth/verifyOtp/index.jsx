import { Flex, Text, Link, PinInput } from "@chakra-ui/react";
import AuthBox from "../authBox";
import ButtonContainer from "../../../sharedComponents/Button";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

const VerifyOtp = () => {
  const [otp, setOtp] = useState(["", "", "", ""]);
  const navigate = useNavigate();

  const handleSubmit = () => {
    navigate("/إنشاء-حساب/نجاح");
  };

  return (
    <AuthBox stepper activeSteps={3}>
      <Flex direction="column" alignItems={"center"} gap={8} mt={8}>
        <Text fontWeight={"semibold"} fontSize={"xl"}>
          تأكيد البريد الإلكتروني
        </Text>
        <Text>ادخال الكود المكون من 4 أرقام المرسل الي البريد الإلكتروني</Text>

        <PinInput.Root value={otp} onValueChange={(e) => setOtp(e.value)} otp>
          <PinInput.Control gap={4} dir="ltr">
            {[0, 1, 2, 3].map((index) => (
              <PinInput.Input
                key={index}
                index={index}
                width={14}
                height={14}
                bg="#D9D9D9"
                _focus={{ bg: "white", borderColor: "blue.500" }}
              />
            ))}
          </PinInput.Control>
        </PinInput.Root>

        <ButtonContainer
          width="full"
          disabled={otp.join("").length < 4}
          direction="column"
          primaryText={"تأكيد"}
          onPrimaryClick={handleSubmit}
        />
        <Link
          as="button"
          color="heading.500"
          fontSize="md"
          _focus={{ outline: "none" }}
          cursor="pointer"
        >
          إعادة إرسال الكود
        </Link>
      </Flex>
    </AuthBox>
  );
};

export default VerifyOtp;
