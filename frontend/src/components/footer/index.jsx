import { Box, Container, Flex, Text } from "@chakra-ui/react";
import { SubText } from "../../sharedComponents/text";
import MapIcon from "../../assets/icons/mapIcon.svg?react";
import MailIcon from "../../assets/icons/mailIcon.svg?react";
import CallIcon from "../../assets/icons/callIcon.svg?react";

const services = [
  { title: "نجارة" },
  { title: "سباكة" },
  { title: "كهرباء" },
  { title: "دهانات" },
  { title: "تركيب سيراميك" },
  { title: "تنجيد الأثاث" },
];
const contact = [
  { key: "العنوان", icon: <MapIcon />, value: "مكرم عبيد, القاهرة، مصر" },
  { key: "اتصل بنا", icon: <CallIcon />, value: "+20 100 123 4567" },
  { icon: <MailIcon />, value: "info@san3a.com" },
];

function Footer() {
  return (
    <Box
      width="100%"
      mt={20}
      p={20}
      backgroundColor={"heading.500"}
      color="white"
    >
      <Flex justifyContent={"space-between"}>
        <Flex direction={"column"} gap={4}>
          <Text fontWeight={"bold"} fontSize={"3xl"}>
            <Text as="span" color={"accent.500"}>
              صن
            </Text>
            <Text as="span">عة</Text>
          </Text>
          <SubText color="white">
            صنّعة هي منصة بتوصلك بأفضل الحرفيين في
            <br />
            كل المجالات، بسهولة وأمان وسرعة في التنفيذ.
          </SubText>
          <SubText mt={"auto"} color="white">
            © 2025 صنّعة — جميع الحقوق محفوظة
          </SubText>
        </Flex>
        <Flex direction={"column"} gap={2}>
          <Text textDecoration={"underline"}>الخدمات</Text>
          {services.map((i) => (
            <Flex>{i.title}</Flex>
          ))}
        </Flex>
        <Flex direction={"column"} gap={2}>
          {contact.map((i) => (
            <Flex gap={4}>
              {i.icon}
              <Text textDecoration={"underline"}>{i.key}</Text>
              <Text>{i.value}</Text>
            </Flex>
          ))}
        </Flex>
      </Flex>
    </Box>
  );
}

export default Footer;
