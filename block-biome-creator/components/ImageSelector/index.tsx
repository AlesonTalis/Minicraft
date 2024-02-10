'use client'

import { V2Type } from "@T/V2"
import { useRef, useState } from "react"

export default function ImageSelector()
{
    const [pos,posSet] = useState<V2Type>({x:0,y:0})
    const [size,sizeSet] = useState<V2Type>({x:0,y:0})
    const image = useRef<HTMLImageElement>(null)

    return (
        <div >
            <div>
                <span>POS: </span>
                <span>X:{pos.x.toString()}</span>
                <span>&nbsp;</span>
                <span>Y:{pos.y.toString()}</span>
            </div>
            <div>
                <span>SIZE: </span>
                <span>X:{size.x.toString()}</span>
                <span>&nbsp;</span>
                <span>Y:{size.y.toString()}</span>
            </div>
            <div className="flex relative flex-row max-h-[50vh]">
                <img 
                    className="object-contain"
                    width={1024}
                    height={512}
                    src="./minecraft_textures_atlas_blocks.png_mipmap_0.png" 
                    alt="blocos"
                    ref={image}
                    onMouseMove={(event) => {
                        let bound = image.current?.getBoundingClientRect()

                        if (bound === undefined) return

                        sizeSet({
                            x: bound.width,
                            y: bound.height
                        })
                        posSet({
                            x: event.clientX - bound.left,
                            y: event.clientY - bound.top
                        })
                    }}
                />
            </div>
        </div>
    )
}