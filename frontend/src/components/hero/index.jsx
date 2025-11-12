import { Box, Flex, Text } from "@chakra-ui/react"
import ImagePlaceholder from "../../sharedComponents/imagePlaceholder"
import IconContainer from "../../sharedComponents/iconContainer"
import CardIcon from "../../assets/icons/card.svg?react"
import CalendarIcon from "../../assets/icons/calendar.svg?react"
import ShieldIcon from "../../assets/icons/shield.svg?react"
import PeopleIcon from "../../assets/icons/people.svg?react"
import ArrowIcon from "../../assets/icons/arrow.svg?react"
import { FontVariants } from "../../theme"
import ButtonContainer from "../../sharedComponents/Button"
import { PrimaryText, SubText } from "../../sharedComponents/text"

const iconBenefits = [

    { label: "سعر ثابت وواضح", icon: <CardIcon /> },
    { label: "حرفيين شُطّار ومعتمدين", icon: <PeopleIcon /> },
    { label: "دفع آمن وسهل", icon: <ShieldIcon /> },
    { label: "خدمة في المعاد بالضبط", icon: <CalendarIcon /> },
]

function Hero() {
    return (
        <Box mt={16} mx={10}>
            <Flex justifyContent={"space-around"}>
                <Flex direction={"column"} gap={8}>
                    <Box {...FontVariants.heading1}>
                        <PrimaryText/>
                        وفر وقتك
                        <Text>وخليك دايمًا مطمن</Text>
                    </Box>
                    <SubText>
                        بدل ما تلف وتسأل مين يعرف سباك شاطر أو نجار مضمون،
                        <br />
                        صنّعـه هيجبلك الحرفي اللي محتاجه في ثواني!
                        <br />
                        كلهم متأكدين ومراجَعين، وتقدر تختار اليوم والمعاد اللي يناسبك،
                        <br />
                        والدفع كمان آمن وسهل .. كله وانت قاعد مكانك
                    </SubText>
                    <Flex>
                        {iconBenefits.map((i, index) => {
                            return (
                                <IconContainer
                                    textWidth={24}
                                    backgroundColor={"#EFF6FF"}
                                    key={index}
                                    icon={i.icon}
                                    title={i.label}
                                    {...FontVariants.smallLabels}
                                />
                            )
                        })}

                    </Flex>

                    <Flex alignItems={"center"} gap={2}>
                        <Text color={"heading.500"} >احجز دلوقتي :</Text>
                        <ButtonContainer
                            icon={<Box as={ArrowIcon} boxSize={3} />}
                            secondaryText="اختار محافظتك"
                            primaryText="اختار الخدمة"
                        />

                    </Flex>

                </Flex>
                <ImagePlaceholder imageId={"hero"} />
            </Flex>
        </Box>
    )
}

export default Hero