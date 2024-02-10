import { V2IType } from "./V2I"

export type ItemSettingsType = {
    itemIndex: number
    itemId: string
    itemName: string
    itemDescription: string|null,
    itemType: number

    itemImageFaces: V2IType[]
    itemOverlayFaces: V2IType[]
}