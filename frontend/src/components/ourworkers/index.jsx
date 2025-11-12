import { Container, Flex, Text } from "@chakra-ui/react"
import { HeadingText, SubText } from "../../sharedComponents/text"
import StarIcon from "../../assets/icons/star.svg?react"
import CalendarAddIcon from "../../assets/icons/calendar_add.svg?react"
import ImagePlaceholder from "../../sharedComponents/imagePlaceholder"

const workersArray = [
    { name: "طارق حنفية", description: "سباك - القاهرة", stars: 4, views: 189, image: <ImagePlaceholder imageId={"person1"} extension="png" imageSize={16} rounded="full" /> },
    { name: "الاسطي عبده البلف", description: "ميكانيكي - الاسكندرية", stars: 5, views: 45, image: <ImagePlaceholder imageId={"person2"} extension="png" imageSize={16} rounded="full" /> },
    { name: "كريم اجنه", description: "نجار - دمياط", stars: 3, views: 35, image: <ImagePlaceholder imageId={"person3"} extension="png" imageSize={16} rounded="full" /> },
]

function OurWorkers() {

    return (
        <Container mb={20}>
            <Flex direction={"column"} gap={8}>

                <HeadingText>حرفيين مختارين بعناية لأفضل تجربة خدمة</HeadingText>
                <Flex my={10} gap={8}>
                    {workersArray.map((i, index) => (
                        <Flex boxShadow="0 4px 6px -1px rgba(0, 0, 0, 0.1)"
                            key={index} backgroundColor={"white"} width="35%" p={6} rounded="xl" gap={4} direction={"column"}>
                            <Flex gap={3}>
                                {[...Array(i.stars)].map((_, i) => <StarIcon key={i} />)}
                                <Text textDecoration={"underline"} color="paragraph.500" mt={4}>({i.views})</Text>
                            </Flex>
                            <Flex gap={4}>
                                {i.image}
                                <Flex justifyContent={"space-between"} direction={"column"}>
                                    <SubText fontWeight="bold">{i.name}</SubText>
                                    <Text color="paragraph.500">{i.description}</Text>
                                </Flex>
                            </Flex>
                            <Flex alignItems={"center"} gap={2}>
                                <Text fontSize={"xl"}>احجز دلوقتي</Text>
                                <CalendarAddIcon />
                            </Flex>
                        </Flex>
                    ))}
                </Flex>
            </Flex>
        </Container>
    )

}

export default OurWorkers