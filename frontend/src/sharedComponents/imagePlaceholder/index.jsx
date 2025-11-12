import { Image } from "@chakra-ui/react";


function ImagePlaceholder({ imageId,alt,rounded, imageSize,extension="webp",...props }) {

    const imageUrl = `/assets/images/${imageId}.${extension}`

    return (
        <Image  src={imageUrl} rounded={rounded} alt={imageId} height={imageSize||"auto"} width={imageSize||"auto"} loading="lazy" draggable="false" {...props} />
    )
}

export default ImagePlaceholder