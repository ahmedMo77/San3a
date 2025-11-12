import { Flex, List, Text } from "@chakra-ui/react"
import Button from "../../sharedComponents/Button"
import ButtonContainer from "../../sharedComponents/Button"

function Navbar() {
    const navItems = [
        { title: "الرئيسية", href: "/" },
        { title: "الخدمات", href: "/الخدمات" },
        { title: "تواصل معنا", href: "/تواصل" },
    ]
    return (
        <Flex justifyContent={"space-between"} mx={32} my={6}>

            <Text fontWeight={"bold"} fontSize={"3xl"}>
                <Text as="span" color={"primary.500"}>صن</Text>
                <Text as="span" color={"accent.500"}>عة</Text>
            </Text>

            <List.Root flexDirection={"row"} listStyle={"none"} gap={10}>
                {navItems.map((item) => (
                    <List.Item key={item.href}>{item.title}</List.Item>
                ))}
            </List.Root>

            <ButtonContainer/>

        </Flex>
    )
}

export default Navbar