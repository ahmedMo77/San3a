import { Container, Flex } from "@chakra-ui/react";
import { HeadingText, PrimaryText, SubText } from "../../sharedComponents/text";
import ButtonContainer from "../../sharedComponents/Button";

function ReadySection() {
    return (
        <Container>
            <Flex direction={"column"} justifyContent={"center"} alignItems={"center"} gap={16}> 

            <HeadingText>
                جاهز تبدأ تجرب
                <PrimaryText />
                ؟
            </HeadingText>
            <SubText>انضم لأكتر من 500 عميل وحرفي بيستخدموا صنّعة كل يوم. ابدأ النهارده بخطوة واحدة.</SubText>
            <ButtonContainer
                secondaryText="سجل كعميل"
                primaryText="سجل كحرفي"
                primaryProps={{bg:"accent.500",_hover:{bg:"accent.600"}}}
            />
            </Flex>
        </Container>
    )
}

export default ReadySection