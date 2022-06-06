import matplotlib.image as mpimg
import matplotlib.pyplot as plt
import cv2

import cv2
import time
image1 = mpimg.imread("001.tif")
image2 = mpimg.imread("002.tif")
#  计算特征点提取&生成描述时间
start = time.time()
sift = cv2.xfeatures2d.SIFT_create()
#  使用SIFT查找关键点key points和描述符descriptors
kp1, des1 = sift.detectAndCompute(image1, None)
kp2, des2 = sift.detectAndCompute(image2, None)
end = time.time()
print("特征点提取&生成描述运行时间:%.2f秒"%(end-start))

kp_image1 = cv2.drawKeypoints(image1, kp1, None)
kp_image2 = cv2.drawKeypoints(image2, kp2, None)

ratio = 0.5

#  计算匹配点匹配时间
start = time.time()

#  K近邻算法求取在空间中距离最近的K个数据点，并将这些数据点归为一类
matcher = cv2.BFMatcher()
raw_matches = matcher.knnMatch(des1, des2, k = 2)
print(len(raw_matches))
good_matches = []
for m1, m2 in raw_matches:
    #  如果最接近和次接近的比值大于一个既定的值，那么我们保留这个最接近的值，认为它和其匹配的点为good_match
    if m1.distance < ratio * m2.distance:
        good_matches.append([m1])
end = time.time()
print(len(good_matches))
print("匹配点匹配运行时间:%.2f秒"%(end-start))

matches = cv2.drawMatchesKnn(image1, kp1, image2, kp2, good_matches, None, flags = 2)

plt.figure()
plt.imshow(matches)
plt.savefig('matches.png',dpi=1000)

import numpy as np

#  单应性矩阵有八个参数，每一个对应的像素点可以产生2个方程(x一个，y一个)，那么需要四个像素点就能解出单应性矩阵
if len(good_matches) > 4:
    #  计算匹配时间
    start = time.time()
    ptsA= np.float32([kp1[m[0].queryIdx].pt for m in good_matches]).reshape(-1, 1, 2)
    ptsB = np.float32([kp2[m[0].trainIdx].pt for m in good_matches]).reshape(-1, 1, 2)
    ransacReprojThreshold = 4
    #  单应性矩阵可以将一张图通过旋转、变换等方式与另一张图对齐
    H, status =cv2.findHomography(ptsA,ptsB,cv2.RANSAC,ransacReprojThreshold);
    imgOut = cv2.warpPerspective(image2, H, (image1.shape[1],image1.shape[0]),flags=cv2.INTER_LINEAR + cv2.WARP_INVERSE_MAP)
    end = time.time()
    print("匹配运行时间:%.2f秒"%(end-start))
    
    plt.figure()
    plt.imshow(image1)
    plt.figure()
    plt.imshow(image2)
    plt.figure()
    plt.imshow(imgOut)
    plt.savefig('imgOut.png', dpi = 300)
