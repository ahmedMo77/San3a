import { Box, Container, Flex, Grid, GridItem, Text } from "@chakra-ui/react";
import { FontVariants } from "../../theme";
import BulbIcon from "../../assets/icons/servicesIcons/ri_lightbulb-flash-line.svg?react"
import WaterIcon from "../../assets/icons/servicesIcons/fa7-solid_faucet-drip.svg?react"
import PaintIcon from "../../assets/icons/servicesIcons/game-icons_paint-roller.svg?react"
import WoodIcon from "../../assets/icons/servicesIcons/fa-solid_hammer.svg?react"
import TrowelIcon from "../../assets/icons/servicesIcons/game-icons_trowel.svg?react"
import FrontLoaderIcon from "../../assets/icons/servicesIcons/front_loader.svg?react"
import SofaIcon from "../../assets/icons/servicesIcons/mdi_sofa.svg?react"
import ConcreteIcon from "../../assets/icons/servicesIcons/game-icons_concrete-bag.svg?react"
import CleaningIcon from "../../assets/icons/servicesIcons/mdi_cleaning.svg?react"
import ConditionerIcon from "../../assets/icons/servicesIcons/mynaui_air-conditioner-solid.svg?react"
import TreeIcon from "../../assets/icons/servicesIcons/tabler_trees.svg?react"
import HouseRoofIcon from "../../assets/icons/servicesIcons/lucide-lab_house-roof.svg?react"
import IconContainer from "../../sharedComponents/iconContainer";
import { HeadingText, PrimaryText } from "../../sharedComponents/text"; 

const ServicesItems = [
    { title: "كهرباء", icon: <BulbIcon /> },
    { title: "سباكة", icon: <WaterIcon /> },
    { title: "دهانات", icon: <PaintIcon /> },
    { title: "نجار", icon: <WoodIcon /> },
    { title: "بلّاط", icon: <TrowelIcon /> },
    { title: "خرسانة", icon: <FrontLoaderIcon /> },
    { title: "منجّد", icon: <SofaIcon /> },
    { title: "بنّاء", icon: <ConcreteIcon /> },
    { title: "نظافة", icon: <CleaningIcon /> },
    { title: "تكييف", icon: <ConditionerIcon /> },
    { title: "زراعه و تنسيق الأشجار", icon: <TreeIcon /> },
    { title: "عزل وسقف", icon: <HouseRoofIcon /> }
]

function Services() {
    return (
        <Container>
            <Flex direction={"column"}>
                    <HeadingText>الخدمات اللي تقدر تطلبها من <PrimaryText/></HeadingText>
                <Box my={16} >
                    <Grid
                        templateColumns="repeat(5, 1fr)"
                        gap={8}
                        justifyItems="center"
                        alignItems="center"
                    >
                        {ServicesItems.slice(0, 10).map((item, index) => (
                            <IconContainer
                                rounded="full"
                                fontSize="xl"
                                fontWeight={"semibold"}
                                width={36}
                                height={36}
                                key={index}
                                icon={item.icon}
                                title={item.title}
                            />
                        ))}
                    </Grid>
                    <Flex justifyContent="center" gap={6} mt={6}>
                        {ServicesItems.slice(10).map((item, index) => (
                            <IconContainer
                                rounded="full"
                                fontSize="xl"
                                fontWeight={"semibold"}
                                width={36}
                                height={36}
                                key={index + 10}
                                icon={item.icon}
                                title={item.title}
                            />
                        ))}
                    </Flex>
                </Box>
            </Flex>
        </Container>
    )
}

export default Services