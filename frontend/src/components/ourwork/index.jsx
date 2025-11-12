import { Box, Container, Flex, Text } from "@chakra-ui/react"
import { HeadingText, PrimaryText, SubText } from "../../sharedComponents/text"
import CheckIcon from "../../assets/icons/check_card.svg?react"
import FillCalendarIcon from "../../assets/icons/fill_calendar.svg?react"
import PaymentIcont from "../../assets/icons/payment.svg?react"
import UserSearchIcon from "../../assets/icons/user_search.svg?react"

const workArray = [
    { number: 1, headline: "دور على الحرفي", icon: <UserSearchIcon />, subLine: "اختار نوع الخدمة اللي محتاجها من بين المئات من الحرفيين." },
    { number: 2, headline: "اختار السعر و المعاد", icon: <FillCalendarIcon />, subLine: "شوف الأسعار وحدد اليوم والوقت اللي يناسبك." },
    { number: 3, headline: "احجز الخدمة بأمان", icon: <PaymentIcont />, subLine: "ادفع أونلاين ، واحجز في دقايق." },
    { number: 4, headline: "استلم شغلك وانت مطمن", icon: <CheckIcon />, subLine: "صنّعة بتضمنلك الجودة والالتزام بالمواعيد." },
]

function OurWork() {

    return (
        <Container>
            <HeadingText>
                إزاي
                <PrimaryText />
                بيشتغل؟
            </HeadingText>
            <Box>
                {workArray.map((i, index) => (
                    <Box key={i.number} width="fit-content"
                        position={"relative"} mr={index % 2 === 0 ? "0" : "30%"}>
                        <Text fontWeight={"bold"} fontSize="200px" color={"#6B7280"} opacity={"18%"}>{i.number}</Text>
                        <Box position={"absolute"} top="52%" right="50%" zIndex={"100"}>
                            <Box boxShadow="0 4px 6px -1px rgba(0, 0, 0, 0.1)"
                                backgroundColor="white" p={4} rounded="xl" width={"500px"} spaceY={4}>
                                <Flex gap={4}>
                                    <Box>{i.icon}</Box>
                                    <SubText fontWeight="bold">{i.headline}</SubText>
                                </Flex>
                                <Text>{i.subLine}</Text>
                            </Box>
                        </Box>
                    </Box>
                    
                ))}

            </Box>
            
        </Container>
    )

}

export default OurWork