import { Flex, List } from "@chakra-ui/react";
import ButtonContainer from "../../sharedComponents/Button";
import { Link, useNavigate } from "react-router-dom";
import Logo from "../../sharedComponents/logo";

function Navbar() {
  const navItems = [
    { title: "الرئيسية", href: "/" },
    { title: "الخدمات", href: "/الخدمات" },
    { title: "تواصل معنا", href: "/تواصل" },
  ];

  const navigate = useNavigate();

  return (
    <Flex justifyContent={"space-between"} mx={32} my={6}>
      <Logo />
      <List.Root flexDirection={"row"} listStyle={"none"} gap={10}>
        {navItems.map((item) => (
          <List.Item key={item.href}>
            <Link to={item.href}>{item.title}</Link>
          </List.Item>
        ))}
      </List.Root>
      <ButtonContainer
        secondaryText="تسجيل دخول"
        primaryText="حساب جديد"
        onPrimaryClick={() => navigate("/إنشاء-حساب")}
        onSecondaryClick={() => navigate("/تسجيل-الدخول")}
      />
    </Flex>
  );
}

export default Navbar;
