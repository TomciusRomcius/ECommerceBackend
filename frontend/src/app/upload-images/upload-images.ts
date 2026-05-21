import { Component, computed, input, signal, WritableSignal } from '@angular/core';
import { MatError } from "@angular/material/form-field";
import { MatIcon } from "@angular/material/icon";

function generateImageId(images: ImageData[]) {
  if (images.length === 0)
    return 0;
  const ids = images.map((img) => img.id);
  return Math.max(...ids) + 1;
}

interface ImageData {
  data64: string;
  id: number;
};

@Component({
  selector: 'app-upload-images',
  imports: [MatError, MatIcon],
  templateUrl: './upload-images.html',
  styleUrl: './upload-images.css',
})
export class UploadImages {
  imagesSignal = input.required<WritableSignal<File[]>>();
  imagesRequired = input.required<boolean>();

  imagesData = signal<ImageData[]>([]);

  async onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    const files: File[] = [];
    for (const file of input.files ?? []) {
      files.push(file);
    }
    const urls = await Promise.all(files.map((file) => this.readFileData(file)));
    this.imagesSignal().set(files);
    const imagesData = this.imagesData();
    // Reset ids
    for (let i = 0; i < imagesData.length; i++) {
      imagesData[i].id = i;
    }

    for (let i = 0; i < files.length; i++) {
      imagesData.push({
        data64: urls[i],
        id: i + imagesData.length
      });
    }

    this.imagesData.set(imagesData);
  }

  deleteImage(id: number) {
    const newImages = this.imagesData().filter((img) => img.id !== id);
    this.imagesData.set(newImages);
  }

  // Returns file data as base64 encoded
  readFileData(file: File): Promise<string> {
    return new Promise((resolve, reject) => {
      const fileReader = new FileReader();
      fileReader.readAsDataURL(file);
      fileReader.onload = (ev) => {
        resolve(ev.target?.result as string);
      };
      fileReader.onerror = () => {
        reject("Failed to open the file");
      };
    })
  }
}
