import { Box, Container, Flex, Text } from "@chakra-ui/react"
import { HeadingText, SubText } from "../../sharedComponents/text"
import StarIcon from "../../assets/icons/star.svg?react"
import ImagePlaceholder from "../../sharedComponents/imagePlaceholder"
import { Button } from "../../sharedComponents/Button"

const FeedBackArray = [
    { name: "محمود حسن - القاهرة", description: "طلبت نجار من صنّعة، وصل في نفس اليوم، الشغل نظيف جدًا والسعر ثابت فعلاً، أنصح بيهم بشدة.", stars: 4, image: <ImagePlaceholder imageId={"person4"} extension="png" imageSize={36} rounded="full" /> },
    { name: "ساره احمد - القليوبيه", description: "طلبت نجار من صنّعة، وصل في نفس اليوم، الشغل نظيف جدًا والسعر ثابت فعلاً، أنصح بيهم بشدة.", stars: 5, image: <ImagePlaceholder imageId={"person5"} extension="png" imageSize={36} rounded="full" /> },
    { name: "كنزي مدبولي - الاسكندرية", description: "طلبت نجار من صنّعة، وصل في نفس اليوم، الشغل نظيف جدًا والسعر ثابت فعلاً، أنصح بيهم بشدة.", stars: 5, image: <ImagePlaceholder imageId={"person6"} extension="png" imageSize={36} rounded="full" /> },
]

function Feedback() {

    return (
        <Container mb={20}>
            <HeadingText> آراء العملاء</HeadingText>
            <SubText my={10} textAlign="center">“اسمع من الناس اللي جربت صنّعة بنفسها قبل ما تبدأ.”</SubText>
            <Flex pb={20} my={10} gap={16}>
                {FeedBackArray.map((i, index) => (
                    <Flex position="relative" height="250px" boxShadow="0 4px 6px -1px rgba(0, 0, 0, 0.1)"
                        key={index} backgroundColor={"white"} p={8} rounded="xl" alignItems={"center"} width="full" direction={"column"} gap={4}>
                        <Text textAlign={"center"} color="paragraph.500">{i.description}</Text>
                        <SubText fontWeight="bold">{i.name}</SubText>
                        <Flex mb={4} gap={3}>
                            {[...Array(i.stars)].map((_, i) => <StarIcon key={i} />)}
                        </Flex>
                        <Box position="absolute" top={"75%"} zIndex={100}>
                            {i.image}
                        </Box>
                    </Flex>
                ))}
            </Flex>
            <Flex gap={4} justifyContent={"center"} alignItems={"center"}>
            <SubText>جرب صنّعة النهارده وشوف بنفسك</SubText>
                    <Button value={"ابدأ دلوقتي"} variant="primary"/>
            </Flex>
        </Container>
    )

}

export default Feedback