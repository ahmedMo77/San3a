import { Container, Grid, Text } from "@chakra-ui/react"
import { HeadingText, PrimaryText, SubText } from "../../sharedComponents/text"
import CheckIcon from "../../assets/icons/checkmark.svg?react"
import CardIcon from "../../assets/icons/card.svg?react"
import CalendarIcon from "../../assets/icons/calendar.svg?react"
import ShieldIcon from "../../assets/icons/shield.svg?react"
import BoxContainer from "../../sharedComponents/boxContainer"
import IconContainer from "../../sharedComponents/iconContainer"

const IconItems = [
    { header: "حرفيين موثوقين", sub: "كل الحرفيين عندنا متأكدين من هويتهم وخبرتهم.", icon: <CheckIcon /> },
    { header: "سعر ثابت وواضح", sub: "السعر اللي تشوفه هو اللي هتدفعه، من غير مفاجآت.", icon: <CardIcon /> },
    { header: "خدمة في المعاد", sub: "نوصلك بالحرفي في اليوم والساعة اللي تختارها.", icon: <CalendarIcon /> },
    { header: "دفع آمن وسهل", sub: "كل عمليات الدفع مؤمنة تمامًا، سواء أونلاين أو كاش.", icon: <ShieldIcon /> },
]

function WhyUs() {

    return (
        <Container>
            <HeadingText>ليه تختار  <PrimaryText/> ؟</HeadingText>
            <SubText my={10} textAlign="center">علشان بنقدملك حرفيين موثوقين، بأسعار ثابتة، ودفع آمن، وخدمة في الوقت اللي يناسبك.</SubText>
            <Grid
                templateColumns="repeat(2, 1fr)"
                gap={8}
                justifyItems="center"
                alignItems="center"
            >
                {IconItems.map((i,index) => {
                    return (
                        <BoxContainer
                            width={96}
                            height={72}
                            key={index}
                        >
                            <IconContainer
                                fontWeight={"bold"}
                                subTextWidth={72}
                                iconSize={20}
                                fontSize={"xl"}
                                key={index}
                                icon={i.icon}
                                title={i.header}
                                sub={i.sub}
                            />
                        </BoxContainer>
                    )
                })}

            </Grid>
        </Container>
    )

}

export default WhyUs