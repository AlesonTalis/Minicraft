import ImageSelector from "@C/ImageSelector";


export default function Editor()
{
    return (
        <div className="flex flex-col min-h-screen p-4">
            <div className="text-2xl">Editor de Blocos</div>
            <ImageSelector />
        </div>
    )
}