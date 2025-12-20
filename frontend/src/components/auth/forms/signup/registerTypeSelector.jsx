import { Flex } from "@chakra-ui/react";
import RegisterClientCard from "../../Tabs/RegisterClientCard";
import RegisterWorkerCard from "../../Tabs/RegisterWorkerCard";
import { useState } from "react";
import AuthBox from "../../authBox";

const RegisterTypeSelector = () => {
  const [hovered, setHovered] = useState(null);

  return (
    <AuthBox stepper>
      <Flex direction="column" gap={4}>
        <Flex justifyContent="center" gap={8}>
          <RegisterClientCard
            faded={hovered === "worker"}
            onHover={() => setHovered("client")}
            onLeave={() => setHovered(null)}
          />
          <RegisterWorkerCard
            faded={hovered === "client"}
            onHover={() => setHovered("worker")}
            onLeave={() => setHovered(null)}
          />
        </Flex>
      </Flex>
    </AuthBox>
  );
};

export default RegisterTypeSelector;
