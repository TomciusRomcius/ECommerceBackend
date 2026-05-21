import { Component, input, signal, WritableSignal } from '@angular/core';
import { MatIcon } from "@angular/material/icon";
import { MatAnchor, MatButton } from "@angular/material/button";

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

const MAX_IMAGES = 6;

@Component({
  selector: 'app-upload-images',
  imports: [MatButton, MatIcon],
  templateUrl: './upload-images.html',
  styleUrl: './upload-images.css',
})
export class UploadImages {
  imagesSignal = input.required<WritableSignal<File[]>>();
  imagesRequired = input.required<boolean>();

  imagesData = signal<ImageData[]>([]);
  error = signal('');

  async onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    const files: File[] = [];
    for (const file of input.files ?? []) {
      files.push(file);
    }

    if (this.imagesData().length + files.length > MAX_IMAGES) {
      this.error.set(`You can upload a maximum of ${MAX_IMAGES} images.`);
      input.value = '';
      return;
    }

    this.error.set('');
    const urls = await Promise.all(files.map((file) => this.readFileData(file)));
    this.imagesSignal().update((existing) => [...existing, ...files]);
    const imagesData = this.getImagesData(files, urls);

    this.imagesData.set(imagesData);
    input.value = '';
  }

  deleteImage(id: number) {
    const newImages = this.imagesData().filter((img) => img.id !== id);
    if (newImages.length <= 6) {
      this.error.set('');
    }
    this.imagesData.set(newImages);
  }

  private getImagesData(files: File[], urls: string[]) {
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
    return imagesData;
  }

  // Returns file data as base64 encoded
  private readFileData(file: File): Promise<string> {
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
