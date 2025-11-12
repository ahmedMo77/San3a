import { Box } from "@chakra-ui/react";

function BoxContainer({ width, height, children, rounded = "lg", stars = false, extraIcons = false }) {

    return (
        <Box
            display="flex"
            alignItems="center"
            justifyContent="center"
            width={width}
            height={height}
            rounded={rounded}
            background={"white"}
            boxShadow="0 4px 6px -1px rgba(0, 0, 0, 0.1)"

        >
            {children}
        </Box>
    )
}

export default BoxContainer